const express = require('express');
const ocr_expenses = require('../services/ocr_expenses');
const router = new express.Router();
 
router.get('/', async (req, res, next) => {
  let options = { 
    "fiscal": req.query.fiscal,
    "userId": req.query.userId,
  };


  try {
    const result = await ocr-expenses.getOCRExpenses(options);
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

  options.createOCRExpenseInlineReqFormdata = req.body;

  try {
    const result = await ocr-expenses.createOCRExpense(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/process', async (req, res, next) => {
  let options = { 
  };

  options.processOCRExpenseInlineReqJson = req.body;

  try {
    const result = await ocr-expenses.processOCRExpense(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/report', async (req, res, next) => {
  let options = { 
    "fiscal": req.query.fiscal,
    "userId": req.query.userId,
  };


  try {
    const result = await ocr-expenses.getOCRExpensesReport(options);
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
    const result = await ocr-expenses.getOCRExpense(options);
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
  };


  try {
    const result = await ocr-expenses.deleteOCRExpense(options);
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

  options.updateOCRExpenseInlineReqFormdata = req.body;

  try {
    const result = await ocr-expenses.updateOCRExpense(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;