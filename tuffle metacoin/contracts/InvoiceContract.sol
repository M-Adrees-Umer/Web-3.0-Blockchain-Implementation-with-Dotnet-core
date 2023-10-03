// SPDX-License-Identifier: MIT
pragma solidity ^0.8.13;

contract InvoiceContract {
    struct Invoice {
        string id;
        string customerName;
        uint256 amount;
    }

    mapping(uint256 => Invoice) public invoices;
    uint256 public invoiceCount;

    event InvoiceCreated(uint256 indexed id, string customerName, uint256 amount);

    function createInvoice(string memory _id, string memory _customerName, uint256 _amount) external {
        Invoice memory newInvoice = Invoice(_id, _customerName, _amount);
        invoices[invoiceCount] = newInvoice;
        emit InvoiceCreated(invoiceCount, _customerName, _amount);
        invoiceCount++;
    }

    function getInvoice(uint256 _id) external view returns (string memory, string memory, uint256) {
        require(_id < invoiceCount, "Invalid invoice ID");
        Invoice memory invoice = invoices[_id];
        return (invoice.id, invoice.customerName, invoice.amount);
    }
}
