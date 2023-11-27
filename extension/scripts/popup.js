document.addEventListener("DOMContentLoaded", function () {
  const contractCodeInput = $("#contractCode");
  const loadContractButton = $("#loadContract");
  const contractImageContainer = $("#contractImageContainer");

  loadContractButton.click(async function () {
    const contractCode = contractCodeInput.val();

    try {
      // Gọi API để lấy đối tượng hợp đồng
      const contractObject = await callContractApi(contractCode);

      // Lấy base64 từ đối tượng
      const base64Pdf = contractObject.pdfBase64;

      // Chuyển base64 thành blob
      var byteCharacters = atob(base64Pdf);
      var byteNumbers = new Array(byteCharacters.length);
      for (var i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }
      var byteArray = new Uint8Array(byteNumbers);
      var blob = new Blob([byteArray], { type: "application/pdf" });

      // Hiển thị PDF dưới dạng hình ảnh bằng pdf.js
      displayPdfAsImage(blob);
    } catch (error) {
      console.error("Error loading contract:", error);
    }
  });

  async function displayPdfAsImage(pdfBlob) {
    // Sử dụng pdf.js để chuyển đổi PDF thành hình ảnh
    const pdfData = await pdfjsLib.getDocument({ data: pdfBlob }).promise;
    const canvas = document.createElement("canvas");
    const context = canvas.getContext("2d");

    // Chọn trang đầu tiên của PDF
    const pdfPage = await pdfData.getPage(1);

    // Thiết lập kích thước của canvas dựa trên kích thước của trang PDF
    canvas.width = pdfPage.view[2];
    canvas.height = pdfPage.view[3];

    // Vẽ hình ảnh PDF lên canvas
    const renderContext = {
      canvasContext: context,
      viewport: pdfPage.view,
    };
    await pdfPage.render(renderContext).promise;

    // Hiển thị hình ảnh trong container
    contractImageContainer.html("");
    contractImageContainer.append(canvas);
  }

  async function callContractApi(contractCode) {
    return new Promise((resolve, reject) => {
      let urlContract = "https://localhost:7286/api/PContract/" + contractCode;
      $.ajax({
        url: urlContract,
        type: "GET",
        success: function (data) {
          resolve({
            pdfBase64: data.base64File,
          });
        },
        error: function (error) {
          reject(error);
        },
      });
    });
  }
});
