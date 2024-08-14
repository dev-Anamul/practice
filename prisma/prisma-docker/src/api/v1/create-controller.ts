import { prisma } from "@/prisma/prisma-client";
import { Request, Response } from "express";

export const createHandler = async (req: Request, res: Response) => {
  const user = await prisma.user.create({
    data: req.body,
  });

  return res.status(200).json({ message: "Create team", user });
};
