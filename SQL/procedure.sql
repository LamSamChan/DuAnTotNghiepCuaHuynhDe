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

--test
CREATE PROCEDURE CountPendingContracts
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
        SUM(CASE WHEN PC.IsCustomer = 0 THEN 1 ELSE 0 END) AS [CustomerNotSigned],
        E.FullName AS [EmployeeName],
        COUNT(PC.EmployeeCreatedId) AS [EmployeeContractCount]
    FROM
        PendingContract PC
		LEFT JOIN TypeOfService TOS ON PC.TOS_ID = TOS.Id
		LEFT JOIN Employee E ON PC.EmployeeCreatedId = E.Id
    WHERE
        PC.DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        TOS.ServiceName,
        CONVERT(DATE, PC.DateCreated),
        E.FullName;
END;


Exec CountPendingContracts '09/11/2023','01/01/2024'