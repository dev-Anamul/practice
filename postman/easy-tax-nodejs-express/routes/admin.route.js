const express = require('express');
const admin = require('../services/admin');
const router = new express.Router();
 
router.get('/dashboard', async (req, res, next) => {
  let options = { 
  };


  try {
    const result = await admin.getDashboardDataForAdmin(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/dashboard/daily-income-expense', async (req, res, next) => {
  let options = { 
    "numOfDays": req.query.numOfDays,
  };


  try {
    const result = await admin.getDailyExpenseForAdmin(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/dashboard/monthly-income-expense', async (req, res, next) => {
  let options = { 
    "numOfMonths": req.query.numOfMonths,
  };


  try {
    const result = await admin.getMonthlyIncomeExpenseForAdminDashboard(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/expenses', async (req, res, next) => {
  let options = { 
    "expand": req.query.expand,
    "fields": req.query.fields,
    "limit": req.query.limit,
    "order": req.query.order,
    "page": req.query.page,
    "params": req.query.params,
    "search": req.query.search,
    "sort": req.query.sort,
  };


  try {
    const result = await admin.getAdminExpenses(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/income-sources', async (req, res, next) => {
  let options = { 
    "fields": req.query.fields,
    "limit": req.query.limit,
    "order": req.query.order,
    "page": req.query.page,
    "params": req.query.params,
    "search": req.query.search,
    "sort": req.query.sort,
  };


  try {
    const result = await admin.getAdminIncomeSources(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/notifications', async (req, res, next) => {
  let options = { 
    "expand": req.query.expand,
    "fields": req.query.fields,
    "limit": req.query.limit,
    "order": req.query.order,
    "page": req.query.page,
    "search": req.query.search,
    "sort": req.query.sort,
  };


  try {
    const result = await admin.getAdminNotifications(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/notifications/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };


  try {
    const result = await admin.getNotificationByIdForAdmin(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/ocr-expenses', async (req, res, next) => {
  let options = { 
    "expand": req.query.expand,
    "fields": req.query.fields,
    "limit": req.query.limit,
    "order": req.query.order,
    "page": req.query.page,
    "search": req.query.search,
    "sort": req.query.sort,
  };


  try {
    const result = await admin.getAdminOCRExpenses(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/users', async (req, res, next) => {
  let options = { 
    "fields": req.query.fields,
    "limit": req.query.limit,
    "order": req.query.order,
    "page": req.query.page,
    "params": req.query.params,
    "search": req.query.search,
    "sort": req.query.sort,
  };


  try {
    const result = await admin.getUsers(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/users', async (req, res, next) => {
  let options = { 
  };

  options.createUserInlineReqJson = req.body;

  try {
    const result = await admin.createUser(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/users/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };


  try {
    const result = await admin.getAdminUserById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.delete('/users/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };


  try {
    const result = await admin.deleteAdminUserById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.patch('/users/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };

  options.updateAdminUserByIdInlineReqJson = req.body;

  try {
    const result = await admin.updateAdminUserById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;