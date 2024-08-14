module.exports = {
  /**
  * Get all income sources of the currently logged in user
  * @param options.fields Fields to be returned   * @param options.limit How many items to return at one time (max 100)   * @param options.order Order by   * @param options.page Current page number   * @param options.search Search by   * @param options.sort Sort by 

  */
  getIncomeSources: async (options) => {

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
  * Create a new income source for the currently logged in user

  * @param options.incomeSourceDto.amount
  * @param options.incomeSourceDto.description
  * @param options.incomeSourceDto.endDate
  * @param options.incomeSourceDto.incomeDate
  * @param options.incomeSourceDto.incomeSource
  * @param options.incomeSourceDto.incomeType
  * @param options.incomeSourceDto.startDate

  */
  createIncomeSource: async (options) => {

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
      status = '201';

    return {
      status: status,
      data: data
    };  
  },

  /**
  * download income sources as csv


  */
  downloadIncomeSourcesAsCsv: async (options) => {

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
  * Get daily Income of a user
  * @param options.numOfDays Number of days 

  */
  getDailyIncome: async (options) => {

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
  * Get total cost by category


  */
  getIncomeByCategory: async (options) => {

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
  * Get monthly Income
  * @param options.numOfMonths number of months 

  */
  getMonthlyIncome: async (options) => {

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
  * Get single income source of the currently logged in user by id
  * @param options.id ID 

  */
  getIncomeSource: async (options) => {

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
  * Delete specific income source of the currently logged in user by id
  * @param options.id ID 

  */
  deleteIncomeSource: async (options) => {

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
  * Update specific income source of the currently logged in user by id
  * @param options.id ID 
  * @param options.incomeSourceDto.amount
  * @param options.incomeSourceDto.description
  * @param options.incomeSourceDto.endDate
  * @param options.incomeSourceDto.incomeDate
  * @param options.incomeSourceDto.incomeSource
  * @param options.incomeSourceDto.incomeType
  * @param options.incomeSourceDto.startDate

  */
  updateIncomeSource: async (options) => {

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
