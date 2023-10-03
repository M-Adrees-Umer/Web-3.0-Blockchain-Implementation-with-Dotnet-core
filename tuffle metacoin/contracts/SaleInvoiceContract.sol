// SPDX-License-Identifier: MIT
pragma solidity ^0.8.13;

contract SaleInvoiceContract {
    struct SaleInvoice {
        string invoiceId;
        string ipfsHash; // IPFS CID representing the JSON data
    }

    mapping(string => SaleInvoice) private saleInvoices;

    function addOrUpdateSaleInvoice(string memory invoiceId, string memory ipfsHash) public {
        SaleInvoice memory newInvoice = SaleInvoice({
            invoiceId: invoiceId,
            ipfsHash: ipfsHash
        });

        saleInvoices[invoiceId] = newInvoice;
    }

    function getSaleInvoice(string memory invoiceId) public view returns (string memory) {
        return saleInvoices[invoiceId].ipfsHash;
    }
}
