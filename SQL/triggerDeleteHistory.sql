CREATE TRIGGER tr_DeleteOldEmpHistoryRecords
ON OperationHistoryEmp
FOR INSERT
AS
BEGIN
    DELETE FROM OperationHistoryEmp
    WHERE Date < DATEADD(MONTH, -3, GETDATE())
END
go

CREATE TRIGGER tr_DeleteOldCusHistoryRecords
ON OperationHistoryCus
FOR INSERT
AS
BEGIN
    DELETE FROM OperationHistoryCus
    WHERE Date < DATEADD(MONTH, -3, GETDATE())
END