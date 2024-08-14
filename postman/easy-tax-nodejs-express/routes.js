module.exports = function (app) {
  /*
   * Routes
   */
  app.use("/api/v1/admin", require("./routes/admin.route"));
  app.use("/api/v1/assets", require("./routes/assets.route"));
  app.use("/api/v1/auth", require("./routes/auth.route"));
  app.use("/api/v1/categories", require("./routes/categories.route"));
  app.use("/api/v1/dashboard", require("./routes/dashboard.route"));
  app.use("/api/v1/depreciation", require("./routes/depreciation.route"));
  app.use("/api/v1/expenses", require("./routes/expenses.route"));
  app.use("/api/v1/fiscal-years", require("./routes/fiscal_years.route"));
  app.use("/api/v1/income-sources", require("./routes/income_sources.route"));
  app.use("/api/v1/income-types", require("./routes/income_types.route"));
  app.use("/api/v1/notifications", require("./routes/notifications.route"));
  app.use("/api/v1/ocr-expenses", require("./routes/ocr_expenses.route"));
  app.use("/api/v1/reports", require("./routes/reports.route"));
  app.use("/api/v1/settings", require("./routes/settings.route"));
  app.use(
    "/api/v1/support-messages",
    require("./routes/support_messages.route")
  );
  app.use("/api/v1/tax", require("./routes/tax.route"));
  app.use("/api/v1/tax-slabs", require("./routes/tax_slabs.route"));
};
