import ExcelJS from "exceljs";

export const exportExcelFileHandler = async () => {
  const workbook = new ExcelJS.Workbook();

  // set workbook properties
  workbook.creator = "Me";
  workbook.lastModifiedBy = "Me";
  workbook.created = new Date();
  workbook.modified = new Date();
  workbook.lastPrinted = new Date();

  // add a worksheet
  const worksheet = workbook.addWorksheet("Lab Requisition Form");

  // add print area
  worksheet.pageSetup.printArea = "A1:AG62";
  worksheet.properties.defaultRowHeight = 18;
  worksheet.properties.defaultColWidth = 5;

  // default font for  the entire worksheet
  worksheet.eachRow((row) => {
    row.eachCell((cell) => {
      cell.font = { name: "Calibri", size: 11 };
    });
  });

  // add logo to the worksheet
  worksheet.getCell("A1").value = "Logo";
  worksheet.mergeCells("A1:B3");
  worksheet.mergeCells("A8:C8");
  worksheet.getCell("A8").value = "Lab Requisition Form";
  worksheet.getCell("A1").alignment = {
    vertical: "middle",
    horizontal: "center",
  };
  worksheet.getCell("A1").style = {
    font: { bold: true, size: 20, color: { argb: "FF000000" } },
    alignment: { vertical: "middle", horizontal: "center" },
    border: {
      top: { style: "double" },
      left: { style: "thin" },
      bottom: { style: "thin" },
      right: { style: "thin" },
    },
    fill: {
      type: "pattern",
      pattern: "solid",
      fgColor: { argb: "FFCCFFCC" },
    },
  };

  // download excel file
  workbook.xlsx.writeBuffer().then((data) => {
    const blob = new Blob([data], {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = url;
    a.download = "Lab Requisition Form.xlsx";
    a.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
  });
};
