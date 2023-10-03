// 2_deploy_sale_invoice_contract.js
const SaleInvoiceContract = artifacts.require("SaleInvoiceContract");

module.exports = function (deployer) {
  deployer.deploy(SaleInvoiceContract);
};
