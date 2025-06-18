SELECT 
    c."Id" AS "CarId",
    c."Model",
    c."LicensePlate",
    LEAST(
        ROUND(
            COALESCE(SUM(EXTRACT(EPOCH FROM (cu."EndTime" - cu."StartTime")) / 3600), 0)
            /
            24.0
            * 100, 2
        ),
    100) AS "UsagePercent"
FROM "Cars" c
LEFT JOIN "CarUsage" cu ON cu."CarId" = c."Id"
GROUP BY c."Id";
