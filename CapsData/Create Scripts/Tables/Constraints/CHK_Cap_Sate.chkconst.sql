﻿ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_Sate] CHECK ([STATE]=(5) OR [STATE]=(4) OR [STATE]=(3) OR [STATE]=(2) OR [STATE]=(1));

