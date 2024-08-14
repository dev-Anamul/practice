import { v4 } from "uuid";

export const addUUIDToBody = (req, res, next) => {
  req.body.id = v4();
  next();
};
