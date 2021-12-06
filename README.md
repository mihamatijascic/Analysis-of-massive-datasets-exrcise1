# Analysis-of-massive-datasets-ex1
First exercise from the course Analysis of massive datasets at University of Zagreb, Faculty of Electrical Engineering and Computing. Task of the exercise is to compress documents 
(in input file, one line represents one document) in way that similar documents have similar hash (SimHash algorithm) and then answer a series of query's. Query's for certain document ask how many other documents have maximal K Hamming distance compared to previous document. In Task A small number of documents is processed and compared. In Task B large
number of documents needs to be processed and compared, because of that, Locality Sensitive Hashing algorithm is used.
