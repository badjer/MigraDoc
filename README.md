MigraDoc
========

Cut down version of PDFsharp and MigraDoc. For performance reasons the following has been removed
- DDL Read/Write ability
- Internal MetaData has been removed, internal reflection has been removed as a result
- RTF support

Upgraded core libs to .Net 4.5

At this stage basic PDF generation works, simple tables should render correctly.
Performance of table rendering has greatly been improved (internally was using a lot of reflection before)

Comments welcome, let me know if you find this useful. At some stage I might look at testing more of the functionality.
