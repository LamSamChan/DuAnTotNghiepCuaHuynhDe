--done

CREATE PROCEDURE CountCustomersByTypeAndDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        CONVERT(DATE, DateAdded) AS [Date],
        COUNT(Id) AS [TotalCustomers],
        SUM(CASE WHEN typeofCustomer = 'Doanh nghiệp' THEN 1 ELSE 0 END) AS [EnterpriseCustomers],
        SUM(CASE WHEN typeofCustomer = 'Cá nhân' THEN 1 ELSE 0 END) AS [IndividualCustomers]
    FROM
        Customer
    WHERE
        DateAdded BETWEEN @StartDate AND @EndDate
    GROUP BY
        CONVERT(DATE, DateAdded);
END;
go


--Pending Contract
CREATE PROCEDURE CountContractsByTypeAndDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        TOS.ServiceName AS [ServiceTypeName],
        CONVERT(DATE, PC.DateCreated) AS [Date],
        COUNT(PC.Id) AS [TotalContracts],
        SUM(CASE WHEN PC.IsDirector = 1 THEN 1 ELSE 0 END) AS [DirectorApproved],
        SUM(CASE WHEN PC.IsRefuse = 1 THEN 1 ELSE 0 END) AS [DirectorRefused],
        SUM(CASE WHEN PC.IsCustomer = 0 THEN 1 ELSE 0 END) AS [CustomerNotSigned]
    FROM
        PendingContract PC
    LEFT JOIN TypeOfService TOS ON PC.TOS_ID = TOS.Id
    WHERE
        PC.DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        TOS.ServiceName,
        CONVERT(DATE, PC.DateCreated);
END;
go

CREATE PROCEDURE SumPendingContractByDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        CONVERT(DATE, DateCreated) AS [Date],
        COUNT(Id) AS [TotalContracts]
    FROM
        PendingContract
    WHERE
        DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        CONVERT(DATE, DateCreated);
END;
go

CREATE PROCEDURE CountContractsByEmployeeAndDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        LEFT(E.Id, 8) + ' - ' + E.FullName AS [EmployeeInfo],
        CONVERT(DATE, PC.DateCreated) AS [Date],
        COUNT(PC.Id) AS [TotalContracts]
    FROM
        PendingContract PC
    LEFT JOIN Employee E ON PC.EmployeeCreatedId = E.Id
    WHERE
        PC.DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        LEFT(E.Id, 8) + ' - ' + E.FullName,
        CONVERT(DATE, PC.DateCreated);
END;

--test

CREATE PROCEDURE CountContractsByEmployeeAndDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        LEFT(E.Id, 8) + ' - ' + E.FullName AS [EmployeeInfo],
        CONVERT(DATE, PC.DateCreated) AS [Date],
        COUNT(PC.Id) AS [TotalContracts]
    FROM
        PendingContract PC
    LEFT JOIN Employee E ON PC.EmployeeCreatedId = E.Id
    WHERE
        PC.DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        LEFT(E.Id, 8) + ' - ' + E.FullName,
        CONVERT(DATE, PC.DateCreated);
END;

Exec CountContractsByEmployeeAndDate '09/11/2023','01/01/2024'

drop procedure CountPendingContracts