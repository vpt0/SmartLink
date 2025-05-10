import uuid
import time

# In-memory session store
sessions = {}

# Session expiry time in seconds (5 minutes)
SESSION_EXPIRY = 300


def generate_unique_link(base_url="http://127.0.0.1:5000/session/"):
    """Generates a unique session link and stores its metadata."""
    session_id = str(uuid.uuid4())
    sessions[session_id] = {
        'created_at': time.time(),
        'approved': False,
        'user': None
    }
    return f"{base_url}{session_id}"


def is_session_valid(session_id):
    """Checks if a session exists and hasn't expired."""
    if session_id not in sessions:
        return False

    if time.time() - sessions[session_id]['created_at'] > SESSION_EXPIRY:
        del sessions[session_id]  # Clean up expired session
        return False

    return True


def set_user(session_id, username):
    """Sets the username for a session."""
    if is_session_valid(session_id):
        sessions[session_id]['user'] = username
        return True
    return False


def approve_session(session_id):
    """Marks a session as approved."""
    if is_session_valid(session_id):
        sessions[session_id]['approved'] = True
        return True
    return False


def get_session_info(session_id):
    """Fetches the session info."""
    if is_session_valid(session_id):
        return sessions[session_id]
    return None


def remove_session(session_id):
    """Removes a session from the store."""
    if session_id in sessions:
        del sessions[session_id]
        return True
    return False
