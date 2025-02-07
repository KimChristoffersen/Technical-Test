-- Tabel for Overmontorer
CREATE TABLE Overmontorer (
    overmontor_Id INT PRIMARY KEY IDENTITY(1,1),
    navn VARCHAR(100) NOT NULL,
    telefonnummer VARCHAR(20) NOT NULL
);

-- Tabel for Montorer
CREATE TABLE Montorer (
    montor_Id INT PRIMARY KEY IDENTITY(1,1),
    navn VARCHAR(100) NOT NULL,
    telefonnummer VARCHAR(20) NOT NULL
);


-- Tabel for Reference
CREATE TABLE MontorOvermontorReference (
    montor_Id INT NOT NULL,
    overmontor_Id INT NOT NULL,
    PRIMARY KEY (montor_Id, overmontor_Id),
    FOREIGN KEY (montor_Id) REFERENCES Montorer(montor_Id) ON DELETE CASCADE,
    FOREIGN KEY (overmontor_Id) REFERENCES Overmontorer(overmontor_Id) ON DELETE CASCADE
);
