# Dockerfile

# Use a base image with Node.js and Nginx, for example
FROM node:18-alpine as build

# Set working directory
WORKDIR /app

# Copy package.json and install dependencies
COPY package.json yarn.lock ./
RUN yarn install

# Copy the rest of the project files
COPY . .

# Build the Vite project
RUN yarn build

# Serve with Nginx or any other server
FROM nginx:alpine

# Copy the built files
COPY --from=build /app/dist /usr/share/nginx/html

# Copy the runtime environment injection script
COPY ./set-env.sh /docker-entrypoint.d/

# Ensure the script has execution permissions
RUN chmod +x /docker-entrypoint.d/set-env.sh

# Expose port 80
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
