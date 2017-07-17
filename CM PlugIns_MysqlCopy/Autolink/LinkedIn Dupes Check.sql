
-- Invalid records
UPDATE c_mastercompanies set COMPANY_STATUS = 'Invalid records'
WHERE COMPANY_STATUS IS NULL AND (LENGTH(TRIM(IFNULL(CONCAT(COMPANY_NAME,ADDRESS_1, ADDRESS_2, COUNTRY),''))) = 0 OR LENGTH(TRIM(IFNULL(COMPANY_NAME,''))) = 0);
  

-- Telephone dupes within the set
UPDATE c_mastercompanies  AS cm3 
INNER JOIN (SELECT * FROM c_mastercompanies cm2 WHERE cm2.MASTER_ID > 
 (SELECT MIN(MASTER_ID) MASTER_ID FROM c_mastercompanies cm1
                      WHERE cm1.SWITCHBOARD = cm2.SWITCHBOARD AND LENGTH(cm1.SWITCHBOARD) > 0 AND cm1.COMPANY_STATUS IS NULL))
  cm ON cm.MASTER_ID = cm3.MASTER_ID
  set cm3.COMPANY_STATUS = 'Telephone dupes within the set'
  WHERE cm3.COMPANY_STATUS IS null;

-- Company, Address1, Address2 and Country dupes within the set
UPDATE c_mastercompanies  AS cm3 
INNER JOIN (SELECT * FROM c_mastercompanies cm2 WHERE cm2.MASTER_ID > 
 (SELECT MIN(MASTER_ID) MASTER_ID FROM c_mastercompanies cm1
                      WHERE CONCAT(cm1.COMPANY_NAME, cm1.ADDRESS_1, cm1.ADDRESS_2, cm1.COUNTRY) = CONCAT(cm2.COMPANY_NAME, cm2.ADDRESS_1, cm2.ADDRESS_2, cm2.COUNTRY) 
                            AND LENGTH(TRIM(IFNULL(CONCAT(COMPANY_NAME,ADDRESS_1, ADDRESS_2, COUNTRY),''))) > 0  
                            AND cm1.COMPANY_STATUS IS NULL))
  cm ON cm.MASTER_ID = cm3.MASTER_ID
  set cm3.COMPANY_STATUS = 'Company, Address1, Address2 and Country dupes within the set'
  WHERE cm3.COMPANY_STATUS IS null;


-- Company and Country dupes within the set
UPDATE c_mastercompanies  AS cm3 
INNER JOIN (SELECT * FROM c_mastercompanies cm2 WHERE cm2.MASTER_ID > 
 (SELECT MIN(MASTER_ID) MASTER_ID FROM c_mastercompanies cm1
                      WHERE CONCAT(cm1.COMPANY_NAME, cm1.COUNTRY) = CONCAT(cm2.COMPANY_NAME, cm2.COUNTRY) 
                            AND LENGTH(TRIM(IFNULL(CONCAT(COMPANY_NAME, COUNTRY),''))) > 0  
                            AND cm1.COMPANY_STATUS IS NULL))
  cm ON cm.MASTER_ID = cm3.MASTER_ID
  set cm3.COMPANY_STATUS = 'Company and Country dupes within the set'
  WHERE cm3.COMPANY_STATUS IS null;

-- Telephone dupes against master
UPDATE c_mastercompanies cm
INNER JOIN inbinb014_mastercompanies im ON cm.SWITCHBOARD = im.SWITCHBOARD
SET  cm.COMPANY_STATUS = 'Telephone dupes against master'
WHERE cm.COMPANY_STATUS IS NULL 
 AND IFnull(cm.SWITCHBOARD,'') > 0;

-- Telephone dupes against master
UPDATE c_mastercompanies cm
INNER JOIN inbinb014_mastercompanies im ON cm.SWITCHBOARD = im.SWITCHBOARD
SET  cm.COMPANY_STATUS = 'Telephone dupes against master'
WHERE cm.COMPANY_STATUS IS NULL 
 AND IFnull(cm.SWITCHBOARD,'') > 0;

-- Company, Address1, Address2 and Country dupes against master
UPDATE c_mastercompanies cm
INNER JOIN inbinb014_mastercompanies im ON CONCAT(cm.COMPANY_NAME, cm.ADDRESS_1, cm.ADDRESS_2, cm.COUNTRY) = CONCAT(im.COMPANY_NAME, im.ADDRESS_1, im.ADDRESS_2, im.COUNTRY)
SET  cm.COMPANY_STATUS = 'Company, Address1, Address2 and Country dupes against master'
WHERE cm.COMPANY_STATUS IS NULL AND LENGTH(TRIM(IFNULL(CONCAT(cm.COMPANY_NAME,cm.ADDRESS_1, cm.ADDRESS_2, cm.COUNTRY),''))) > 0;

-- Company and Country dupes against master
UPDATE c_mastercompanies cm
INNER JOIN inbinb014_mastercompanies im ON CONCAT(cm.COMPANY_NAME, cm.COUNTRY) = CONCAT(im.COMPANY_NAME, im.COUNTRY)
SET  cm.COMPANY_STATUS = 'Company and Country dupes against master'
WHERE cm.COMPANY_STATUS IS NULL AND LENGTH(TRIM(IFNULL(CONCAT(cm.COMPANY_NAME, cm.COUNTRY),''))) > 0;

