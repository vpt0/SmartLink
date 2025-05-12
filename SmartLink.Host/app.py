from flask import Flask, jsonify, request, render_template
from flask_cors import CORS
from ram_detection import get_ram_info
from cpu_detection import get_cpu_info
from storage_detection import get_storage_info
from system_monitor import get_system_stats

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
            'Health Check': '/api/health'
        }
    })

@app.route('/api/ram', methods=['GET'])
def ram_info():
    """
    API endpoint to provide RAM information.
    """
    try:
        total_ram, free_ram = get_ram_info()
        
        # Extract numeric values by removing " GB" from the end
        total_num = float(total_ram.split()[0])
        free_num = float(free_ram.split()[0])
        
        # Calculate used RAM and usage percentage
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
        return jsonify({
            'status': 'error',
            'message': str(e)
        }), 500

@app.route('/api/cpu', methods=['GET'])
def cpu_info():
    """
    API endpoint to provide CPU information.
    """
    try:
        cpu_data = get_cpu_info()
        
        return jsonify({
            'status': 'success',
            'data': cpu_data
        })
    except Exception as e:
        return jsonify({
            'status': 'error',
            'message': str(e)
        }), 500

@app.route('/api/storage', methods=['GET'])
def storage_info():
    """
    API endpoint to provide storage information.
    """
    try:
        storage_data = get_storage_info()
        
        return jsonify({
            'status': 'success',
            'data': storage_data
        })
    except Exception as e:
        return jsonify({
            'status': 'error',
            'message': str(e)
        }), 500

@app.route('/api/system', methods=['GET'])
def system_overview():
    """
    API endpoint to provide a comprehensive system overview including RAM, CPU, and storage.
    """
    try:
        # Get RAM info
        total_ram, free_ram = get_ram_info()
        total_num = float(total_ram.split()[0])
        free_num = float(free_ram.split()[0])
        used_ram = f"{round(total_num - free_num, 2)} GB"
        ram_usage_percentage = round(((total_num - free_num) / total_num) * 100, 1)
        
        # Get CPU info
        cpu_data = get_cpu_info()
        
        # Get storage info
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
        return jsonify({
            'status': 'error',
            'message': str(e)
        }), 500

@app.route('/api/monitor', methods=['GET'])
def real_time_monitor():
    """
    API endpoint to provide real-time system monitoring data for dashboard display.
    This endpoint uses the integrated system_monitor module to get comprehensive stats.
    """
    try:
        # Get comprehensive system stats from the system_monitor module
        system_stats = get_system_stats()
        
        return jsonify({
            'status': 'success',
            'timestamp': system_stats['timestamp'],
            'data': system_stats
        })
    except Exception as e:
        return jsonify({
            'status': 'error',
            'message': str(e)
        }), 500

@app.route('/api/health', methods=['GET'])
def health_check():
    """Simple health check endpoint to verify the API is running."""
    return jsonify({
        'status': 'ok',
        'message': 'System monitoring API is running'
    })

@app.route('/dashboard', methods=['GET'])
def system_dashboard():
    """Render the system monitoring dashboard."""
    return render_template('system_dashboard.html')

if __name__ == '__main__':
    # For development only - change to proper configuration for production
    app.run(debug=True, host='0.0.0.0', port=5000)