module.exports = {
  /**
  * Get all expenses for the currently logged in user
  * @param options.expand Expand the response with the specified fields   * @param options.fields Fields to be returned   * @param options.limit How many items to return at one time (max 100)   * @param options.order Order by   * @param options.page Current page number   * @param options.search Search by   * @param options.sort Sort by   * @param options.userId ID of the user 

  */
  getAssets: async (options) => {

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
        "data": "<object>",
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
  * Delete specific asset by id
  * @param options.id ID 

  */
  deleteAssetById: async (options) => {

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
        "errors": "<array>",
        "message": "<string>",
      },
      status = '200';

    return {
      status: status,
      data: data
    };  
  },

  /**
  * Update specific asset by id
  * @param options.id ID 
  * @param options.assetDto.category
  * @param options.assetDto.description
  * @param options.assetDto.isSold
  * @param options.assetDto.name
  * @param options.assetDto.purchaseDate
  * @param options.assetDto.purchasePrice
  * @param options.assetDto.saleDate
  * @param options.assetDto.salePrice
  * @param options.assetDto.status

  */
  updateAssetById: async (options) => {

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
        "errors": "<array>",
        "message": "<string>",
      },
      status = '200';

    return {
      status: status,
      data: data
    };  
  },

  /**
  * get all depreciation based on assets id
  * @param options.id ID 

  */
  getDepreciationByAssetId: async (options) => {

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
};
