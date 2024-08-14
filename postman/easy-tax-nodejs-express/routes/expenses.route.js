const express = require('express');
const expenses = require('../services/expenses');
const router = new express.Router();
 
router.get('/', async (req, res, next) => {
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
    const result = await expenses.getExpenses(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/', async (req, res, next) => {
  let options = { 
  };

  options.createExpenseInlineReqFormdata = req.body;

  try {
    const result = await expenses.createExpense(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/bulk', async (req, res, next) => {
  let options = { 
  };

  options.createBulkExpensesInlineReqJson = req.body;

  try {
    const result = await expenses.createBulkExpenses(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/csv', async (req, res, next) => {
  let options = { 
  };


  try {
    const result = await expenses.downloadCsvExpense(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/total-by-category', async (req, res, next) => {
  let options = { 
  };


  try {
    const result = await expenses.getExpensesTotalByCategory(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };


  try {
    const result = await expenses.getExpense(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.delete('/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
    "userId": req.query.userId,
  };


  try {
    const result = await expenses.deleteExpense(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.patch('/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };

  options.updateExpenseInlineReqJson = req.body;

  try {
    const result = await expenses.updateExpense(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;