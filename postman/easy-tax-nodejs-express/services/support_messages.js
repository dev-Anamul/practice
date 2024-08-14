module.exports = {
  /**
  * Get all support messages
  * @param options.expand Expand the response with the specified fields   * @param options.fields Fields to be returned   * @param options.limit How many items to return at one time (max 100)   * @param options.order Order by   * @param options.page Current page number   * @param options.search Search by   * @param options.sort Sort by   * @param options.type type of message 

  */
  getAllSupportMessages: async (options) => {

    // Implement your business logic here...
    //
    // Return all 2xx and 4xx as follows:
    //
    // return {
    //   status: 'statusCode',
    //   data: 'response'
    // }

    // If an error happens during your business logic implementation,
    // you can throw it as follows:
    //
    // throw new Error('<Error message>'); // this will result in a 500

    var data = {
        "code": "<int32>",
        "message": "<string>",
        "status": "<string>",
      },
      status = '200';

    return {
      status: status,
      data: data
    };  
  },

  /**
  * Create a new support message

  * @param options.supportMessageDto.email
  * @param options.supportMessageDto.message
  * @param options.supportMessageDto.subject

  */
  createSupportMessage: async (options) => {

    // Implement your business logic here...
    //
    // Return all 2xx and 4xx as follows:
    //
    // return {
    //   status: 'statusCode',
    //   data: 'response'
    // }

    // If an error happens during your business logic implementation,
    // you can throw it as follows:
    //
    // throw new Error('<Error message>'); // this will result in a 500

    var data = {
        "code": "<int32>",
        "message": "<string>",
        "status": "<string>",
      },
      status = '201';

    return {
      status: status,
      data: data
    };  
  },

  /**
  * Get single support message
  * @param options.id ID 

  */
  getSupportMessageById: async (options) => {

    // Implement your business logic here...
    //
    // Return all 2xx and 4xx as follows:
    //
    // return {
    //   status: 'statusCode',
    //   data: 'response'
    // }

    // If an error happens during your business logic implementation,
    // you can throw it as follows:
    //
    // throw new Error('<Error message>'); // this will result in a 500

    var data = {
        "code": "<int32>",
        "message": "<string>",
        "status": "<string>",
      },
      status = '200';

    return {
      status: status,
      data: data
    };  
  },

  /**
  * Delete single support message
  * @param options.id ID 

  */
  deleteSupportMessageById: async (options) => {

    // Implement your business logic here...
    //
    // Return all 2xx and 4xx as follows:
    //
    // return {
    //   status: 'statusCode',
    //   data: 'response'
    // }

    // If an error happens during your business logic implementation,
    // you can throw it as follows:
    //
    // throw new Error('<Error message>'); // this will result in a 500

    var data = {
        "code": "<int32>",
        "data": "<SupportMessage>",
        "links": "<object>",
        "message": "<string>",
        "status": "<string>",
      },
      status = '200';

    return {
      status: status,
      data: data
    };  
  },

  /**
  * update support message featured
  * @param options.id ID 
  * @param options.updateSupportMessageFeaturedInlineReqJson.featured required

  */
  updateSupportMessageFeatured: async (options) => {

    // Implement your business logic here...
    //
    // Return all 2xx and 4xx as follows:
    //
    // return {
    //   status: 'statusCode',
    //   data: 'response'
    // }

    // If an error happens during your business logic implementation,
    // you can throw it as follows:
    //
    // throw new Error('<Error message>'); // this will result in a 500

    var data = {
        "code": "<int32>",
        "message": "<string>",
        "status": "<string>",
      },
      status = '200';

    return {
      status: status,
      data: data
    };  
  },

  /**
  * message read
  * @param options.id ID 

  */
  readSupportMessage: async (options) => {

    // Implement your business logic here...
    //
    // Return all 2xx and 4xx as follows:
    //
    // return {
    //   status: 'statusCode',
    //   data: 'response'
    // }

    // If an error happens during your business logic implementation,
    // you can throw it as follows:
    //
    // throw new Error('<Error message>'); // this will result in a 500

    var data = {
        "code": "<int32>",
        "message": "<string>",
        "status": "<string>",
      },
      status = '200';

    return {
      status: status,
      data: data
    };  
  },

  /**
  * Give reply to support message
  * @param options.id ID 

  */
  replySupportMessage: async (options) => {

    // Implement your business logic here...
    //
    // Return all 2xx and 4xx as follows:
    //
    // return {
    //   status: 'statusCode',
    //   data: 'response'
    // }

    // If an error happens during your business logic implementation,
    // you can throw it as follows:
    //
    // throw new Error('<Error message>'); // this will result in a 500

    var data = {
        "code": "<int32>",
        "message": "<string>",
        "status": "<string>",
      },
      status = '201';

    return {
      status: status,
      data: data
    };  
  },
};
