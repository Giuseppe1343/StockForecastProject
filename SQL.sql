DECLARE @stokno INT = 79965;

SELECT TOP 1 stokno AS [Id],aciklama AS [Name] FROM MergedTable WHERE stokno = @stokno; 


--Weekly
SELECT tarih AS [TDate], SUM(COALESCE(miktar,0.0)) AS [TAmount]  FROM model_data.dbo.MergedTable WHERE tipi = 761 AND stokno = @stokno AND tarih >= '2023-01-01' GROUP BY tarih  ORDER BY tarih


SELECT stokno ,tarih AS [TDate], COUNT(miktar) AS [TAmount]  FROM model_data.dbo.MergedTable WHERE tipi = 761 AND tarih >= '2023-01-01' GROUP BY tarih,stokno  ORDER BY stokno,tarih

SELECT Tb.Id, (SELECT min(aciklama) FROM MergedTable x WHERE x.stokno=Tb.ID)aciklama, COUNT(Tb.TAmount)'Count'
FROM (
SELECT stokno AS [Id] ,tarih AS [TDate], COUNT(miktar) AS [TAmount]  FROM model_data.dbo.MergedTable WHERE tipi = 761 AND stokno=79965 AND tarih >= '2023-01-01' GROUP BY tarih,stokno
) AS Tb
--JOIN MergedTable as Bs ON Tb.Id = Bs.stokno
GROUP BY Tb.[Id]

--Edited
SELECT stokno AS [Id] 
FROM ( 
	SELECT Tb.stokno, COUNT((SELECT SubTb.tarih, COUNT(SubTb.miktar) FROM MergedTable AS SubTb WHERE SubTb.tipi = 761 AND SubTb.tarih >= '2023-01-01' AND SubTb.stokno = Tb.stokno GROUP BY SubTb.tarih)) AS [Count]
	FROM MergedTable AS Tb
	GROUP BY Tb.stokno
) R
WHERE [Count] > 500

--Original
SELECT stokno AS [Id] ,(SELECT TOP 1 aciklama FROM MergedTable x WHERE x.stokno=R.stokno) AS [Name]
FROM ( 
	SELECT stokno, COUNT(Tb.TAmount) AS [Count]
	FROM (
		SELECT stokno, tarih, COUNT(miktar) AS [TAmount]
		FROM model_data.dbo.MergedTable
		WHERE tipi = 761 AND tarih >= '2023-01-01'
		GROUP BY tarih,stokno
	) Tb
	GROUP BY stokno
) R
WHERE [Count] > 500


SELECT t.stokno, t.TAmount FROM (SELECT DISTINCT stokno, COUNT(miktar) as TAmount FROM MergedTable WHERE tipi = 761 GROUP BY stokno) AS t WHERE t.TAmount > 3000