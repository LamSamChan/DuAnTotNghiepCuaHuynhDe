CREATE TRIGGER tr_DeleteOldEmpHistoryRecords
ON OperationHistoryEmp
FOR INSERT
AS
BEGIN
    DELETE FROM OperationHistoryEmp
    WHERE Date < DATEADD(WEEK, -2, GETDATE())
END
go

CREATE TRIGGER tr_DeleteOldCusHistoryRecords
ON OperationHistoryCus
FOR INSERT
AS
BEGIN
    DELETE FROM OperationHistoryCus
    WHERE Date < DATEADD(WEEK, -2, GETDATE())
END