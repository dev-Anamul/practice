const express = require('express');
const categories = require('../services/categories');
const router = new express.Router();
 
router.get('/', async (req, res, next) => {
  let options = { 
    "limit": req.query.limit,
    "order": req.query.order,
    "page": req.query.page,
    "search": req.query.search,
    "sort": req.query.sort,
  };


  try {
    const result = await categories.getCategories(options);
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

  options.categoryDto = req.body;

  try {
    const result = await categories.createCategory(options);
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
    const result = await categories.getCategory(options);
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
    const result = await categories.deleteCategory(options);
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

  options.categoryDto = req.body;

  try {
    const result = await categories.updateCategory(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/:id/expenses', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
    "expand": req.query.expand,
    "limit": req.query.limit,
    "order": req.query.order,
    "page": req.query.page,
    "sort": req.query.sort,
  };


  try {
    const result = await categories.getExpensesByCategory(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;