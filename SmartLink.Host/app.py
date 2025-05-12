from flask import Flask, jsonify, request, render_template
from flask_cors import CORS
from ram_detection import get_ram_info
from cpu_detection import get_cpu_info
from storage_detection import get_storage_info
from system_monitor import get_system_stats

# Additional imports for configuration summary
import os
import json
from config_summary import generate_config_summary, format_config_as_json

app = Flask(__name__)
# Enable CORS to allow frontend to access the API
CORS(app)

@app.route('/', methods=['GET'])
def root():
    """Root endpoint that lists available API endpoints."""
    return jsonify({
        'status': 'ok',
        'message': 'System monitoring API is running',
        'available_endpoints': {
            'RAM Information': '/api/ram',
            'CPU Information': '/api/cpu',
            'Storage Information': '/api/storage',
            'System Overview': '/api/system',
            'Real-time Monitoring': '/api/monitor',
            'Health Check': '/api/health',
            'Dashboard': '/dashboard',
            'Config Summary': {
                'Generate and Save': '/api/config/summary (POST)',
                'Retrieve by ID': '/api/config/summary/<session_id>',
                'List All': '/api/config/summaries',
                'Generate Only': '/api/config/generate (POST)'
            }
        }
    })

@app.route('/api/ram', methods=['GET'])
def ram_info():
    """API endpoint to provide RAM information."""
    try:
        total_ram, free_ram = get_ram_info()
        total_num = float(total_ram.split()[0])
        free_num = float(free_ram.split()[0])
        used_ram = f"{round(total_num - free_num, 2)} GB"
        usage_percentage = round(((total_num - free_num) / total_num) * 100, 1)

        return jsonify({
            'status': 'success',
            'data': {
                'total_ram': total_ram,
                'free_ram': free_ram,
                'used_ram': used_ram,
                'usage_percentage': usage_percentage
            }
        })
    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)}), 500

@app.route('/api/cpu', methods=['GET'])
def cpu_info():
    """API endpoint to provide CPU information."""
    try:
        cpu_data = get_cpu_info()
        return jsonify({'status': 'success', 'data': cpu_data})
    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)}), 500

@app.route('/api/storage', methods=['GET'])
def storage_info():
    """API endpoint to provide storage information."""
    try:
        storage_data = get_storage_info()
        return jsonify({'status': 'success', 'data': storage_data})
    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)}), 500

@app.route('/api/system', methods=['GET'])
def system_overview():
    """API endpoint to provide a comprehensive system overview."""
    try:
        total_ram, free_ram = get_ram_info()
        total_num = float(total_ram.split()[0])
        free_num = float(free_ram.split()[0])
        used_ram = f"{round(total_num - free_num, 2)} GB"
        ram_usage_percentage = round(((total_num - free_num) / total_num) * 100, 1)

        cpu_data = get_cpu_info()
        storage_data = get_storage_info()

        return jsonify({
            'status': 'success',
            'data': {
                'ram': {
                    'total_ram': total_ram,
                    'free_ram': free_ram,
                    'used_ram': used_ram,
                    'usage_percentage': ram_usage_percentage
                },
                'cpu': cpu_data,
                'storage': storage_data
            }
        })
    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)}), 500

@app.route('/api/monitor', methods=['GET'])
def real_time_monitor():
    """API endpoint to provide real-time system monitoring data."""
    try:
        system_stats = get_system_stats()
        return jsonify({
            'status': 'success',
            'timestamp': system_stats['timestamp'],
            'data': system_stats
        })
    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)}), 500

@app.route('/api/health', methods=['GET'])
def health_check():
    """Simple health check endpoint to verify the API is running."""
    return jsonify({'status': 'ok', 'message': 'System monitoring API is running'})

@app.route('/dashboard', methods=['GET'])
def system_dashboard():
    """Render the system monitoring dashboard."""
    return render_template('system_dashboard.html')

@app.route('/api/config/summary', methods=['POST'])
def create_config_summary():
    """Create and save a configuration summary based on selected CPU and RAM."""
    try:
        data = request.json
        if 'cpu_cores' not in data or 'ram_amount' not in data:
            return jsonify({
                'status': 'error',
                'message': 'Missing required fields: cpu_cores and ram_amount'
            }), 400

        config = generate_config_summary(data['cpu_cores'], float(data['ram_amount']))
        os.makedirs('configs', exist_ok=True)
        config_path = os.path.join('configs', f"{config['session_id']}.json")
        with open(config_path, 'w') as f:
            f.write(format_config_as_json(config))

        return jsonify(config)
    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)}), 500

@app.route('/api/config/summary/<session_id>', methods=['GET'])
def get_config_summary(session_id):
    """Retrieve a specific configuration summary by session ID."""
    try:
        config_path = os.path.join('configs', f"{session_id}.json")
        if not os.path.exists(config_path):
            return jsonify({
                'status': 'error',
                'message': f'Configuration with session ID {session_id} not found'
            }), 404

        with open(config_path, 'r') as f:
            config = json.load(f)
        return jsonify(config)
    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)}), 500

@app.route('/api/config/summaries', methods=['GET'])
def list_config_summaries():
    """List all saved configuration summaries."""
    try:
        os.makedirs('configs', exist_ok=True)
        config_files = [f for f in os.listdir('configs') if f.endswith('.json')]
        configs = []
        for file in config_files:
            with open(os.path.join('configs', file), 'r') as f:
                configs.append(json.load(f))
        return jsonify(configs)
    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)}), 500

@app.route('/api/config/generate', methods=['POST'])
def generate_config():
    """Generate a configuration summary without saving to file."""
    try:
        data = request.json
        if 'selected_cores' not in data or 'selected_ram_gb' not in data:
            return jsonify({
                'status': 'error',
                'message': 'Missing required fields: selected_cores and selected_ram_gb'
            }), 400

        config = generate_config_summary(data['selected_cores'], float(data['selected_ram_gb']))
        return jsonify({
            'status': 'success',
            'data': {
                'config': config
            }
        })
    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)}), 500

if __name__ == '__main__':
    # For development only - change to proper configuration for production
    app.run(debug=True, host='0.0.0.0', port=5000)
