import { User, UserSchema } from "@/schemas/user-schema";
import { Request, Response } from "express";

let users: User[] = [];

export const getUsers = (_req: Request, res: Response) => {
  res.json(users);
};

export const getUser = (req: Request, res: Response) => {
  const userId = req.params.id;
  const user = users.find((u) => u.id === userId);
  if (!user) {
    return res.status(404).json({ error: "User not found" });
  }
  res.json(user);
};

export const createUser = async (req: Request, res: Response) => {
  const data = UserSchema.safeParse(req.body);
  if (!data.success) return res.status(400).json({ error: data.error.errors });
  const newUser: User = { ...data.data };
  users.push(newUser);
  res.status(201).json(newUser);
};

export const updateUser = (req: Request, res: Response) => {
  const userId = req.params.id;
  const updatedData = UserSchema.safeParse(req.body);
  if (!updatedData.success) {
    return res.status(400).json({ error: updatedData.error.errors });
  }
  const userIndex = users.findIndex((u) => u.id === userId);

  if (userIndex === -1) {
    return res.status(404).json({ error: "User not found" });
  }
  const updatedUser: User = { ...updatedData.data };

  users[userIndex] = updatedUser;
  res.json(updatedUser);
};

export const deleteUser = (req: Request, res: Response) => {
  const userId = req.params.id;
  users = users.filter((u) => u.id !== userId);
  res.json({ message: "User deleted" });
};
