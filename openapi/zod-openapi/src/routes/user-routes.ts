import {
  createUser,
  deleteUser,
  getUser,
  getUsers,
  updateUser,
} from "@/controllers/user-controllers";
import { addUUIDToBody } from "@/middlewares/add-uuid";
import express from "express";

const router = express.Router();

router.get("/users", getUsers);
router.get("/users/:id", getUser);
router.post("/users", addUUIDToBody, createUser);
router.put("/users/:id", updateUser);
router.delete("/users/:id", deleteUser);

export default router;
