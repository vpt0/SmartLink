from flask import Flask, jsonify, request
from flask_cors import CORS
from ram_detection import get_ram_info

app = Flask(__name__)
# Enable CORS to allow frontend to access the API
CORS(app)

@app.route('/', methods=['GET'])
def root():
    """Root endpoint that lists available API endpoints."""
    return jsonify({
        'status': 'ok',
        'message': 'RAM monitoring API is running',
        'available_endpoints': {
            'RAM Information': '/api/ram',
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

@app.route('/api/health', methods=['GET'])
def health_check():
    """Simple health check endpoint to verify the API is running."""
    return jsonify({
        'status': 'ok',
        'message': 'RAM API is running'
    })

if __name__ == '__main__':
    # For development only - change to proper configuration for production
    app.run(debug=True, host='0.0.0.0', port=5000)