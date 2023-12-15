--done
use HUYNHDE_DATN_2023
go

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




CREATE PROCEDURE SumPendingContractByDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateCreated, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [Totals]
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

CREATE PROCEDURE CountPendingContractByWeekAndMonth
    @MonthYear NVARCHAR(7) -- Ví dụ: '2023-09'
AS
BEGIN
    SELECT
        ROW_NUMBER() OVER (ORDER BY DATEPART(ISO_WEEK, DateCreated)) AS [Date], -- Sử dụng ROW_NUMBER()
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        FORMAT(DateCreated, 'yyyy-MM') = @MonthYear
    GROUP BY
        DATEPART(ISO_WEEK, DateCreated)
    ORDER BY
        [Date]; -- Sắp xếp theo số thứ tự của tuần
END;
go

CREATE PROCEDURE CountPContractMonthsInLastSixMonths
    @ReferenceMonth DATE -- Thay đổi kiểu dữ liệu của tham số này
AS
BEGIN
    SELECT
        FORMAT(DateCreated, 'yyyy-MM') AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        DateCreated BETWEEN DATEADD(MONTH, -5, @ReferenceMonth) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, @ReferenceMonth))
    GROUP BY
        FORMAT(DateCreated, 'yyyy-MM')
    ORDER BY
        FORMAT(DateCreated, 'yyyy-MM'); -- Sắp xếp theo tháng tăng dần
END;
go





CREATE PROCEDURE SumPendingContractWaitCusByDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateCreated, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        (DateCreated BETWEEN @StartDate AND @EndDate) AND
        (IsCustomer = 0) AND
        (IsRefuse <> 1 OR IsRefuse IS NULL) 
    GROUP BY
        FORMAT(DateCreated, 'dd/MM/yyyy')
	ORDER BY
        MIN(DateCreated);
END;
go

CREATE PROCEDURE CountPendingContractWaitCusByWeekAndMonth
    @MonthYear NVARCHAR(7) -- Ví dụ: '2023-09'
AS
BEGIN
    SELECT
        ROW_NUMBER() OVER (ORDER BY DATEPART(ISO_WEEK, DateCreated)) AS [Date], -- Sử dụng ROW_NUMBER()
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        (FORMAT(DateCreated, 'yyyy-MM') = @MonthYear) AND
        (IsCustomer = 0) AND
        (IsRefuse <> 1 OR IsRefuse IS NULL) 
    GROUP BY
        DATEPART(ISO_WEEK, DateCreated)
    ORDER BY
        [Date]; -- Sắp xếp theo số thứ tự của tuần
END;
go

CREATE PROCEDURE CountPContractWaitCusMonthsInLastSixMonths
    @ReferenceMonth DATE -- Thay đổi kiểu dữ liệu của tham số này
AS
BEGIN
    SELECT
        FORMAT(DateCreated, 'yyyy-MM') AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        PendingContract
    WHERE
        (DateCreated BETWEEN DATEADD(MONTH, -5, @ReferenceMonth) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, @ReferenceMonth))) AND
        (IsCustomer = 0) AND
        (IsRefuse <> 1 OR IsRefuse IS NULL) 
    GROUP BY
        FORMAT(DateCreated, 'yyyy-MM')
    ORDER BY
        FORMAT(DateCreated, 'yyyy-MM'); -- Sắp xếp theo tháng tăng dần
END;
go




CREATE PROCEDURE SumDoneContractByDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateDone, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        DateDone BETWEEN @StartDate AND @EndDate
    GROUP BY
        FORMAT(DateDone, 'dd/MM/yyyy')
	ORDER BY
        MIN(DateDone);
END;
go

CREATE PROCEDURE CountDoneContractByWeekAndMonth
    @MonthYear NVARCHAR(7) -- Ví dụ: '2023-09'
AS
BEGIN
    SELECT
        ROW_NUMBER() OVER (ORDER BY DATEPART(ISO_WEEK, DateDone)) AS [Date], -- Sử dụng ROW_NUMBER()
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        FORMAT(DateDone, 'yyyy-MM') = @MonthYear
    GROUP BY
        DATEPART(ISO_WEEK, DateDone)
    ORDER BY
        [Date]; -- Sắp xếp theo số thứ tự của tuần
END;
go

CREATE PROCEDURE CountDContractMonthsInLastSixMonths
    @ReferenceMonth DATE -- Thay đổi kiểu dữ liệu của tham số này
AS
BEGIN
    SELECT
        FORMAT(DateDone, 'yyyy-MM') AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        DateDone BETWEEN DATEADD(MONTH, -5, @ReferenceMonth) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, @ReferenceMonth))
    GROUP BY
        FORMAT(DateDone, 'yyyy-MM')
    ORDER BY
        FORMAT(DateDone, 'yyyy-MM'); -- Sắp xếp theo tháng tăng dần
END;
go



CREATE PROCEDURE SumUnEffectContractByDate
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SELECT
        MIN(FORMAT(DateUnEffect, 'dd/MM/yyyy')) AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        DateDone BETWEEN @StartDate AND @EndDate
    GROUP BY
        FORMAT(DateUnEffect, 'dd/MM/yyyy')
	ORDER BY
        MIN(DateUnEffect);
END;
go

CREATE PROCEDURE CountUnEffectContractByWeekAndMonth
    @MonthYear NVARCHAR(7) -- Ví dụ: '2023-09'
AS
BEGIN
    SELECT
        ROW_NUMBER() OVER (ORDER BY DATEPART(ISO_WEEK, DateUnEffect)) AS [Date], -- Sử dụng ROW_NUMBER()
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        FORMAT(DateUnEffect, 'yyyy-MM') = @MonthYear
    GROUP BY
        DATEPART(ISO_WEEK, DateUnEffect)
    ORDER BY
        [Date]; -- Sắp xếp theo số thứ tự của tuần
END;
go

CREATE PROCEDURE CountUnEffectContractMonthsInLastSixMonths
    @ReferenceMonth DATE -- Thay đổi kiểu dữ liệu của tham số này
AS
BEGIN
    SELECT
        FORMAT(DateUnEffect, 'yyyy-MM') AS [Date],
        COUNT(Id) AS [Totals]
    FROM
        DoneContract
    WHERE
        DateDone BETWEEN DATEADD(MONTH, -5, @ReferenceMonth) AND DATEADD(DAY, -1, DATEADD(MONTH, 1, @ReferenceMonth))
    GROUP BY
        FORMAT(DateUnEffect, 'yyyy-MM')
    ORDER BY
        FORMAT(DateUnEffect, 'yyyy-MM'); -- Sắp xếp theo tháng tăng dần
END;
go



--test


Exec CountPContractWaitCusMonthsInLastSixMonths '2023/12/01'
go

Exec CountPContractMonthsInLastSixMonths '2023/12/01'
go

select * from PendingContract Where DateCreated Between '2023-10-30' and '2023-11-01'



Exec CountMonthsInLastSixMonths '2023-12-01'
go

drop procedure CountCustomersByWeekAndMonth