import express, { Request, Response, NextFunction } from "express";
import cors from "cors";
import morgan from "morgan";
import dotenv from "dotenv";
import router from "./routes/user-routes";
import * as swaggerUi from "swagger-ui-express";
import openApiSpec from "./openapi";

// env configuration
dotenv.config();

// initialize express
const app = express();

// Middlewares
app.use([express.json(), cors(), morgan("dev")]);

// health check
app.get("/health", (_req, res) => {
  console.log("user Id", _req.headers["x-user-id"]);

  res
    .status(200)
    .json({ status: `${process.env.SERVICE_NAME} service is up and running` });
});

// docs route
app.use("/api/docs", swaggerUi.serve, swaggerUi.setup(openApiSpec));

// routes
app.use("/api/v1", router);

// 404 error handler
app.use((_req: Request, res: Response) => {
  res.status(404).json({ message: "Resource not found" });
});

// global error handler
app.use((error: Error, _req: Request, res: Response, _next: NextFunction) => {
  res.status(500).json({ message: error.message });
});

// define port
const PORT = process.env.PORT || 5001;

// start server
app.listen(PORT, () => {
  console.log(`${process.env.SERVICE_NAME} is up and running on port ${PORT}`);
});
