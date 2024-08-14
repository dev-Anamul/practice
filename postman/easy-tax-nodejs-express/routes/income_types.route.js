const express = require('express');
const income_types = require('../services/income_types');
const router = new express.Router();
 
router.get('/', async (req, res, next) => {
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
    const result = await income-types.getIncomeType(options);
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

  options.createIncomeTypeInlineReqJson = req.body;

  try {
    const result = await income-types.createIncomeType(options);
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
    const result = await income-types.getIncomeTypeById(options);
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
    const result = await income-types.deleteIncomeTypeById(options);
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

  options.incomeTypesDto = req.body;

  try {
    const result = await income-types.updateIncomeTypeById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;