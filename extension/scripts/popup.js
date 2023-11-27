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
      console.log(base64Pdf);
      // Chuyển base64 thành file PDF và hiển thị
      //await showContractPdf(base64Pdf);
    } catch (error) {
      console.error("Error loading contract:", error);
    }
  });

  async function callContractApi(contractCode) {
    let urlContract = "https://localhost:7286/api/PContract/" + contractCode;
    $.ajax({
      url: urlContract,
      type: "GET",
      success: function (data) {
        return {
          pdfBase64: data.base64File,
        };
      },
      error: function (error) {
        console.log(`Error ${error}`);
      },
    });
  }

  async function showContractPdf(base64Pdf) {
    // Chuyển base64 thành blob
    const byteCharacters = atob(base64Pdf);
    const byteArrays = [];
    for (let offset = 0; offset < byteCharacters.length; offset += 512) {
      const slice = byteCharacters.slice(offset, offset + 512);
      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }
    const blob = new Blob(byteArrays, { type: "application/pdf" });

    // Hiển thị nội dung PDF bằng PDF.js
    const pdfUrl = URL.createObjectURL(blob);
    const loadingTask = pdfjsLib.getDocument(pdfUrl);
    try {
      const pdfDoc = await loadingTask.promise;
      const [page] = await pdfDoc.getPage(1);

      // Xóa ảnh cũ và thêm ảnh mới
      contractImageContainer.html("");
      const canvas = document.createElement("canvas");
      const scale = 1.5;
      const viewport = page.getViewport({ scale });
      canvas.width = viewport.width;
      canvas.height = viewport.height;
      const context = canvas.getContext("2d");
      const renderContext = {
        canvasContext: context,
        viewport: viewport,
      };
      await page.render(renderContext).promise;
      contractImageContainer.append(canvas);
    } catch (error) {
      console.error("Error rendering PDF:", error);
    }
  }
});
