module.exports = {
  /**
  * Get all settings


  */
  getAllSettings: async (options) => {

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
  * Create or update a setting

  * @param options.settingDto.appDescriptionApp description
  * @param options.settingDto.appTitleApp title
  * @param options.settingDto.fevIconFavicon
  * @param options.settingDto.fiscalEndMonthFiscal end month
  * @param options.settingDto.fiscalStartMontFiscal start month
  * @param options.settingDto.mobileLogoApp logo
  * @param options.settingDto.titleWeb title
  * @param options.settingDto.webLogoWeb logo

  */
  createOrUpdateSetting: async (options) => {

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
