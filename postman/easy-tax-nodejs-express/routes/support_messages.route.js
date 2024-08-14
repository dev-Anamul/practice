const express = require('express');
const support_messages = require('../services/support_messages');
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
    "type": req.query.type,
  };


  try {
    const result = await support-messages.getAllSupportMessages(options);
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

  options.supportMessageDto = req.body;

  try {
    const result = await support-messages.createSupportMessage(options);
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
    const result = await support-messages.getSupportMessageById(options);
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
    const result = await support-messages.deleteSupportMessageById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/:id/featured', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };

  options.updateSupportMessageFeaturedInlineReqJson = req.body;

  try {
    const result = await support-messages.updateSupportMessageFeatured(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/:id/read', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };


  try {
    const result = await support-messages.readSupportMessage(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/:id/reply', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };

  options.replySupportMessageInlineReqJson = req.body;

  try {
    const result = await support-messages.replySupportMessage(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;