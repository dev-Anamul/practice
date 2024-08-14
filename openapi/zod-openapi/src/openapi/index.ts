import {
  UserSchema,
  createUserSchema,
  updateUserSchema,
} from "@/schemas/user-schema";

import { createDocument } from "zod-openapi";
const openApiSpec = createDocument({
  openapi: "3.0.0",
  info: {
    title: "My API",
    version: "1.0.0",
    description: "My API description",
    contact: {
      name: "API Support",
      url: "http://www.example.com/support",
      email: "apisupport@gmail.com",
    },
    license: {
      name: "Apache 2.0",
      url: "https://www.apache.org/licenses/LICENSE-2.0.html",
    },
    termsOfService: "http://www.example.com/terms/",
  },
  tags: [
    {
      name: "User",
      description: "API for users in the system",
    },
  ],
  paths: {
    "/api/v1/users": {
      get: {
        tags: ["User"],
        summary: "Get all users",
        operationId: "getUsers",
        responses: {
          "200": {
            description: "List of users",
            content: {
              "application/json": {
                schema: {
                  type: "array",
                  items: {
                    $ref: "#/components/schemas/User",
                  },
                },
              },
            },
          },
        },
      },
      post: {
        tags: ["User"],
        summary: "Create a new user",
        operationId: "createUser",
        requestBody: {
          required: true,
          content: {
            "application/json": {
              schema: {
                $ref: "#/components/schemas/CreateUser",
              },
            },
          },
        },
        responses: {
          "201": {
            description: "New user created",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/User",
                },
              },
            },
          },
          "401": {
            $ref: "#/components/responses/UnauthorizedError",
          },
        },
      },
    },
  },
  servers: [],
  components: {
    securitySchemes: {
      bearerAuth: {
        type: "http",
        scheme: "bearer",
        bearerFormat: "JWT",
      },
    },
    parameters: {},
    requestBodies: {},
    responses: {
      UnauthorizedError: {
        content: {
          "application/json": {
            schema: {
              type: "object",
              properties: {
                message: {
                  type: "string",
                },
              },
            },
          },
        },
        description: "Access token is missing or invalid",
      },
      NotFoundError: {
        content: {
          "application/json": {
            schema: {
              type: "object",
              properties: {
                message: {
                  type: "string",
                },
              },
            },
          },
        },
        description: "Resource not found",
      },
      InternalServerError: {
        content: {
          "application/json": {
            schema: {
              type: "object",
              properties: {
                message: {
                  type: "string",
                },
              },
            },
          },
        },
        description: "Internal server error",
      },
    },
    schemas: {
      User: UserSchema,
      CreateUser: createUserSchema,
      updateUser: updateUserSchema,
    },
  },
  externalDocs: {
    url: "https://example.com",
    description: "Find more info here",
  },
});

export default openApiSpec;
