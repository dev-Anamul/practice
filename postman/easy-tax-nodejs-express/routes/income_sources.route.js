const express = require('express');
const income_sources = require('../services/income_sources');
const router = new express.Router();
 
router.get('/', async (req, res, next) => {
  let options = { 
    "fields": req.query.fields,
    "limit": req.query.limit,
    "order": req.query.order,
    "page": req.query.page,
    "search": req.query.search,
    "sort": req.query.sort,
  };


  try {
    const result = await income-sources.getIncomeSources(options);
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

  options.incomeSourceDto = req.body;

  try {
    const result = await income-sources.createIncomeSource(options);
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
    const result = await income-sources.downloadIncomeSourcesAsCsv(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/daily-income', async (req, res, next) => {
  let options = { 
    "numOfDays": req.query.numOfDays,
  };


  try {
    const result = await income-sources.getDailyIncome(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/income-by-category', async (req, res, next) => {
  let options = { 
  };


  try {
    const result = await income-sources.getIncomeByCategory(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/monthly-income', async (req, res, next) => {
  let options = { 
    "numOfMonths": req.query.numOfMonths,
  };


  try {
    const result = await income-sources.getMonthlyIncome(options);
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
    const result = await income-sources.getIncomeSource(options);
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
    const result = await income-sources.deleteIncomeSource(options);
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

  options.incomeSourceDto = req.body;

  try {
    const result = await income-sources.updateIncomeSource(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;