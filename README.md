HealthChatbotAPI

This API is the foundational endpoint for initializing a chatbot session for a user. This API creates a session entry in the database, logs the session start time, and returns a Session ID that will be used for tracking subsequent interactions with the chatbot. This implementation ensures that the system can handle multiple users and maintain a session context across multiple interactions.
