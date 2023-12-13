--done

CREATE PROCEDURE CountCustomersByTypeAndDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateAdded, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [TotalCustomers],
        SUM(CASE WHEN typeofCustomer = N'Doanh nghiệp' THEN 1 ELSE 0 END) AS [EnterpriseCustomers],
        SUM(CASE WHEN typeofCustomer = N'Cá nhân' THEN 1 ELSE 0 END) AS [IndividualCustomers]
    FROM
        Customer
    WHERE
        CAST(DateAdded AS DATE) BETWEEN @StartDate AND @EndDate
    GROUP BY
         FORMAT(DateAdded, 'dd/MM/yyyy')
	ORDER BY
        MIN(DateAdded); -- Sắp xếp theo ngày tăng dần
END;
go


CREATE PROCEDURE CountCustomersByWeekAndMonth
    @MonthYear NVARCHAR(7) -- Ví dụ: '2023-09'
AS
BEGIN
    SELECT
        ROW_NUMBER() OVER (ORDER BY DATEPART(ISO_WEEK, DateAdded)) AS [Date], -- Sử dụng ROW_NUMBER()
        COUNT(Id) AS [TotalCustomers],
        SUM(CASE WHEN typeofCustomer = N'Doanh nghiệp' THEN 1 ELSE 0 END) AS [EnterpriseCustomers],
        SUM(CASE WHEN typeofCustomer = N'Cá nhân' THEN 1 ELSE 0 END) AS [IndividualCustomers]
    FROM
        Customer
    WHERE
        FORMAT(DateAdded, 'yyyy-MM') = @MonthYear
    GROUP BY
        DATEPART(ISO_WEEK, DateAdded)
    ORDER BY
        [Date]; -- Sắp xếp theo số thứ tự của tuần
END;
go

CREATE PROCEDURE CountMonthsInLastSixMonths
    @ReferenceMonth DATE -- Thay đổi kiểu dữ liệu của tham số này
AS
BEGIN
    SELECT
        FORMAT(DateAdded, 'yyyy-MM') AS [Date],
        COUNT(Id) AS [TotalCustomers],
        SUM(CASE WHEN typeofCustomer = N'Doanh nghiệp' THEN 1 ELSE 0 END) AS [EnterpriseCustomers],
        SUM(CASE WHEN typeofCustomer = N'Cá nhân' THEN 1 ELSE 0 END) AS [IndividualCustomers]
    FROM
        Customer
    WHERE
        DateAdded BETWEEN DATEADD(MONTH, -5, @ReferenceMonth) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, @ReferenceMonth))
    GROUP BY
        FORMAT(DateAdded, 'yyyy-MM')
    ORDER BY
        FORMAT(DateAdded, 'yyyy-MM'); -- Sắp xếp theo tháng tăng dần
END;
go

--chua done
--Pending Contract
CREATE PROCEDURE CountContractsByTypeAndDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        TOS.ServiceName AS [ServiceTypeName],
        MIN(FORMAT(PC.DateCreated, 'dd/MM/yyyy')) AS [Date],
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
        FORMAT(PC.DateCreated, 'dd/MM/yyyy')
	ORDER BY
        MIN(PC.DateCreated);
END;
go

CREATE PROCEDURE SumPendingContractByDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateCreated, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [TotalContracts]
    FROM
        PendingContract
    WHERE
        DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        FORMAT(DateCreated, 'dd/MM/yyyy')
	ORDER BY
        MIN(DateCreated);
END;
go

CREATE PROCEDURE CountContractsByEmployeeAndDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        LEFT(E.Id, 8) + ' - ' + E.FullName AS [EmployeeInfo],
        MIN(FORMAT(PC.DateCreated, 'dd/MM/yyyy')) AS [Date],
        COUNT(PC.Id) AS [TotalContracts]
    FROM
        PendingContract PC
    LEFT JOIN Employee E ON PC.EmployeeCreatedId = E.Id
    WHERE
        PC.DateCreated BETWEEN @StartDate AND @EndDate
    GROUP BY
        LEFT(E.Id, 8) + ' - ' + E.FullName,
        FORMAT(PC.DateCreated, 'dd/MM/yyyy')
	ORDER BY
        MIN(PC.DateCreated);
END;
go

--Done Contract

CREATE PROCEDURE CountTotalContractsInEffect
    @Start DATETIME2,
    @End DATETIME2
AS
BEGIN
    SELECT
	    MIN(FORMAT(DoneContract.DateDone, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [TotalContractsInEffect]
    FROM
        DoneContract
    WHERE
        IsInEffect = 1
        AND DateDone BETWEEN @Start AND @End
    GROUP BY
		FORMAT(DoneContract.DateDone, 'dd/MM/yyyy')
	ORDER BY
        MIN(DoneContract.DateDone);
END;
go


CREATE PROCEDURE CountContractsInEffectByService
    @Start DATETIME2,
    @End DATETIME2
AS
BEGIN
    SELECT
	    MIN(FORMAT(DC.DateDone, 'dd/MM/yyyy')) AS [Date],
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
        TOS.ServiceName
	ORDER BY
        MIN(DC.DateDone);
END;
go

CREATE PROCEDURE CountContractsInEffectByEmployee
    @Start DATETIME2,
    @End DATETIME2
AS
BEGIN
    SELECT
	    MIN(FORMAT(DC.DateDone, 'dd/MM/yyyy'))  AS [Date],
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
        LEFT(E.Id, 8) + ' - ' + E.FullName
	ORDER BY
        MIN(DC.DateDone);
END;
go


CREATE PROCEDURE CountTotalContractsUnEffect
    @Start DATETIME2,
    @End DATETIME2
AS
BEGIN
    SELECT
	   MIN(FORMAT(DoneContract.DateUnEffect, 'dd/MM/yyyy')) AS [Date],
       COUNT(Id) AS [TotalContractsInEffect]
    FROM
        DoneContract
    WHERE
        IsInEffect = 0
        AND DateDone BETWEEN @Start AND @End
    GROUP BY
	 FORMAT(DoneContract.DateUnEffect, 'dd/MM/yyyy')
	 ORDER BY
        MIN(DoneContract.DateUnEffect);
END;
go

CREATE PROCEDURE CountContractsNotInEffectByService
    @Start DATETIME2,
    @End DATETIME2
AS
BEGIN
    SELECT
	    MIN(FORMAT(DC.DateUnEffect, 'dd/MM/yyyy')) AS [Date],
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
        TOS.ServiceName
	ORDER BY
        MIN(DC.DateUnEffect);
END;
go
--test




Exec CountMonthsInLastSixMonths '2023-12-01'
go

drop procedure CountCustomersByWeekAndMonth