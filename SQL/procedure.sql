--done

CREATE PROCEDURE CountCustomersByTypeAndDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        FORMAT(DateAdded, 'dd/MM/yyyy') AS [Date],
        COUNT(Id) AS [TotalCustomers],
        SUM(CASE WHEN typeofCustomer = N'Doanh nghiệp' THEN 1 ELSE 0 END) AS [EnterpriseCustomers],
        SUM(CASE WHEN typeofCustomer = N'Cá nhân' THEN 1 ELSE 0 END) AS [IndividualCustomers]
    FROM
        Customer
    WHERE
        CAST(DateAdded AS DATE) BETWEEN @StartDate AND @EndDate
    GROUP BY
         FORMAT(DateAdded, 'dd/MM/yyyy')
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
        FORMAT(PC.DateCreated, 'dd/MM/yyyy') AS [Date],
        COUNT(PC.Id) AS [TotalContracts],
        SUM(CASE WHEN PC.IsDirector = 1 THEN 1 ELSE 0 END) AS [DirectorApproved],
        SUM(CASE WHEN PC.IsRefuse = 1 THEN 1 ELSE 0 END) AS [DirectorRefused],
        SUM(CASE WHEN PC.IsRefuse = 0 AND PC.IsCustomer = 0 AND PC.IsDirector = 1 THEN 1 ELSE 0 END) AS [CustomerNotSigned],
        SUM(CASE WHEN PC.IsDirector = 0 AND PC.IsRefuse = 0 THEN 1 ELSE 0 END) AS [DirectorNotChecked]
    FROM
        PendingContract PC
    LEFT JOIN TypeOfService TOS ON PC.TOS_ID = TOS.Id
    WHERE
        PC.DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        TOS.ServiceName,
        FORMAT(PC.DateCreated, 'dd/MM/yyyy');
END;
go

CREATE PROCEDURE SumPendingContractByDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        FORMAT(DateCreated, 'dd/MM/yyyy') AS [Date],
        COUNT(Id) AS [TotalContracts]
    FROM
        PendingContract
    WHERE
        DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        FORMAT(DateCreated, 'dd/MM/yyyy');
END;
go

CREATE PROCEDURE CountContractsByEmployeeAndDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        LEFT(E.Id, 8) + ' - ' + E.FullName AS [EmployeeInfo],
        FORMAT(PC.DateCreated, 'dd/MM/yyyy') AS [Date],
        COUNT(PC.Id) AS [TotalContracts]
    FROM
        PendingContract PC
    LEFT JOIN Employee E ON PC.EmployeeCreatedId = E.Id
    WHERE
        PC.DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        LEFT(E.Id, 8) + ' - ' + E.FullName,
        FORMAT(PC.DateCreated, 'dd/MM/yyyy');
END;
go

--Done Contract

CREATE PROCEDURE CountTotalContractsInEffect
    @Start DATETIME2,
    @End DATETIME2
AS
BEGIN
    SELECT
	    FORMAT(DoneContract.DateDone, 'dd/MM/yyyy') AS [Date],
        COUNT(Id) AS [TotalContractsInEffect]
    FROM
        DoneContract
    WHERE
        IsInEffect = 1
        AND DateDone BETWEEN @Start AND @End
    GROUP BY
	FORMAT(DoneContract.DateDone, 'dd/MM/yyyy');
END;
go


CREATE PROCEDURE CountContractsInEffectByService
    @Start DATETIME2,
    @End DATETIME2
AS
BEGIN
    SELECT
	    FORMAT(DC.DateDone, 'dd/MM/yyyy') AS [Date],
        TOS.ServiceName AS [ServiceName],
        COUNT(DC.Id) AS [TotalContractsInEffect]
    FROM
        DoneContract DC
    LEFT JOIN TypeOfService TOS ON DC.TOS_ID = TOS.Id
    WHERE
        DC.IsInEffect = 1
        AND DC.DateDone BETWEEN @Start AND @End
    GROUP BY
	    FORMAT(DC.DateDone, 'dd/MM/yyyy') ,
        TOS.ServiceName;
END;
go

CREATE PROCEDURE CountContractsInEffectByEmployee
    @Start DATETIME2,
    @End DATETIME2
AS
BEGIN
    SELECT
	    FORMAT(DC.DateDone, 'dd/MM/yyyy')  AS [Date],
        LEFT(E.Id, 8) + ' - ' + E.FullName AS [EmployeeInfo],
        COUNT(DC.Id) AS [TotalContractsInEffect]
    FROM
        DoneContract DC
    LEFT JOIN Employee E ON DC.EmployeeCreatedId = E.Id
    WHERE
        DC.IsInEffect = 1
        AND DC.DateDone BETWEEN @Start AND @End
    GROUP BY
	    FORMAT(DC.DateDone, 'dd/MM/yyyy'),
        LEFT(E.Id, 8) + ' - ' + E.FullName;
END;
go


CREATE PROCEDURE CountTotalContractsUnEffect
    @Start DATETIME2,
    @End DATETIME2
AS
BEGIN
    SELECT
	   FORMAT(DoneContract.DateUnEffect, 'dd/MM/yyyy') AS [Date],
       COUNT(Id) AS [TotalContractsInEffect]
    FROM
        DoneContract
    WHERE
        IsInEffect = 0
        AND DateDone BETWEEN @Start AND @End
    GROUP BY
	 FORMAT(DoneContract.DateUnEffect, 'dd/MM/yyyy');
END;
go

CREATE PROCEDURE CountContractsNotInEffectByService
    @Start DATETIME2,
    @End DATETIME2
AS
BEGIN
    SELECT
	    FORMAT(DC.DateUnEffect, 'dd/MM/yyyy') AS [Date],
        TOS.ServiceName AS [ServiceName],
        COUNT(DC.Id) AS [TotalContractsNotInEffect]
    FROM
        DoneContract DC
    LEFT JOIN TypeOfService TOS ON DC.TOS_ID = TOS.Id
    WHERE
        DC.IsInEffect = 0
        AND DC.DateUnEffect BETWEEN @Start AND @End
    GROUP BY
	    FORMAT(DC.DateUnEffect, 'dd/MM/yyyy'),
        TOS.ServiceName;
END;
go
--test




Exec CountContractsByTypeAndDate '09/11/2022','01/01/2024'

drop procedure CountCustomersByTypeAndDate