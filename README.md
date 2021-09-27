# EBookGenerator
電子書產生器


## Epub 新增頁面修改重點
1. <br> => <br />
1. <p> =>  <p class="indent-2em">
1. <h2> => <h2 id="toc_index_%n" class="align-center">
1. `standard.opf` 中新增
  ```
    <!-- xhtml -->
    ...
    <item media-type="application/xhtml+xml" id="p-002"  href="xhtml/p-002.xhtml"/>

    ...
    <!-- font -->
    ...
    <itemref linear="yes" idref="p-002"/>
  ```

1. `p-toc.xhtml` 中新增頁面項目
  ```
  ...
  	<p><a href="p-002.xhtml#toc_index_1"><span>Day1 Why DevOps?</span></a></p>
  ```
1. `navigation-documents.xhtml` 中新增目錄項目
  ```
  	<li><a href="xhtml/p-002.xhtml#toc_index_1"><span>Day1 Why DevOps?</span></a></li>
  ```
