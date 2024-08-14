import { NextRequest, NextResponse } from "next/server";

export const middleware = (request: NextRequest) => {
  console.log("i am running from middleware");
  return NextResponse.redirect(new URL("/", request.url));
};

export const config = {
  matcher: ["/middleware"],
};
