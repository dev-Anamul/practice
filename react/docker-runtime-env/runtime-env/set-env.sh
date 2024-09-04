#!/bin/sh

# Create a config.js file with environment variables
echo "window.RUNTIME_ENV = {" > /usr/share/nginx/html/config.js

# Add the VITE_API_URL variable
echo "  VITE_API_URL: '${VITE_API_URL}'," >> /usr/share/nginx/html/config.js

# Add another environment variable
echo "  VITE_ANOTHER_VAR: '${VITE_ANOTHER_VAR}'," >> /usr/share/nginx/html/config.js

# Add yet another environment variable
echo "  VITE_YET_ANOTHER_VAR: '${VITE_YET_ANOTHER_VAR}'," >> /usr/share/nginx/html/config.js

# Close the object in config.js
echo "};" >> /usr/share/nginx/html/config.js

# Run the server
exec "$@"
