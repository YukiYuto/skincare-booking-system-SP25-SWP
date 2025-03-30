/* eslint-disable */
import React from "react";
import { Table } from "antd";

const DynamicTable = ({ columns, data, onRowSelect }) => {
  const rowSelection = {
    onChange: (selectedRowKeys, selectedRows) => {
      if (onRowSelect) {
        onRowSelect(selectedRows);
      }
    },
  };

  return (
    <Table
      rowSelection={rowSelection}
      columns={columns}
      dataSource={data}
      rowKey="id"
      pagination={{ pageSize: 5 }}
    />
  );
};

export default DynamicTable;
