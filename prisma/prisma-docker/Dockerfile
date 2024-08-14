FROM node:20-alpine 

WORKDIR /app

COPY package.json yarn.lock ./

RUN yarn install

COPY . .

EXPOSE 5003

RUN npx prisma generate
RUN npx prisma migrate deploy

# Set the DATABASE_URL environment variable
# ENV DATABASE_URL="postgresql://postgres:postgres@db:5432/test-db?schema=public"

# ENTRYPOINT [ "yarn" ,"migrate:dev"  ]
CMD ["yarn", "dev"]