CREATE PROCEDURE FI_SP_VerificarBeneficiarioDuplicado
    @CPF VARCHAR(14),
    @IDCLIENTE BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM BENEFICIARIOS WHERE CPF = @CPF AND IDCLIENTE = @IDCliente)
    BEGIN
        -- Se já existir um beneficiário com o mesmo CPF para o mesmo cliente, lança um erro.
        RAISERROR('CPF duplicado para este cliente.', 16, 1);
    END
END