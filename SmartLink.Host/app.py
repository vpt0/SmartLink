from flask import Flask, jsonify
from ram_detection import get_ram_info

app = Flask(__name__)

@app.route('/api/ram', methods=['GET'])
def ram_info():
    total_ram, free_ram = get_ram_info()
    return jsonify({'total_ram': total_ram, 'free_ram': free_ram})

if __name__ == '__main__':
    app.run(debug=True)