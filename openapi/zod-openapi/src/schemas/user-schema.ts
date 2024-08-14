import { z } from "zod";

export const UserSchema = z.object({
  id: z.string().uuid({ message: "Invalid user id" }),
  name: z
    .string({ invalid_type_error: "Name must be a string" })
    .min(3)
    .max(50, { message: "Name must be between 3 and 50 characters" }),
  email: z.string().email(),
  age: z
    .number({ invalid_type_error: "Age must be a number" })
    .positive({ message: "Age must be positive number" })
    .int({ message: "Age must be a positive integer" }),
});

export const createUserSchema = UserSchema.pick({
  name: true,
  email: true,
  age: true,
});

export const updateUserSchema = UserSchema.partial({
  name: true,
  email: true,
  age: true,
});

export type User = z.infer<typeof UserSchema>;
