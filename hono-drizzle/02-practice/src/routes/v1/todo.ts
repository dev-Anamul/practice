import { Hono } from "hono";

const book = new Hono();

book.get("/", (c) => {
  return c.json({ message: "get all the books" });
});

book.get("/:id", (c) => {
  return c.json({ message: "get a book by id" });
});

book.post("/", (c) => {
  return c.json({ message: "create a book" });
});

book.put("/:id", (c) => {
  return c.json({ message: "update a book by id" });
});

book.delete("/:id", (c) => {
  return c.json({ message: "delete a book by id" });
});

export default book;
