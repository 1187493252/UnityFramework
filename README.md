# UnityFramework

#源Unity项目增加Hook文件pre-commit
</br>#功能:在提交前将Samples文件夹的文件及文件夹复制到Samples~ 文件夹里,提交Samples~ 文件夹里的文件
</br>#项目已忽略Samples文件夹,通过修改其他文件触发hook,从而提交Samples文件夹的修改

#pre-commit代码
</br>#!/bin/sh

#指定源文件夹和目标文件夹
</br>SOURCE_FOLDER="Samples"
</br>TARGET_FOLDER="Samples~"

#确保目标文件夹存在
</br>mkdir -p "$TARGET_FOLDER"

#清空目标文件夹中的所有文件和子文件夹
</br>rm -rf "$TARGET_FOLDER"/*

#复制源文件夹及其内容到目标文件夹
</br>cp -r "$SOURCE_FOLDER"/. "$TARGET_FOLDER"/

#添加目标文件夹中的所有文件和子文件夹到暂存区
</br>git add "$TARGET_FOLDER"

#提交信息（可选）
</br>#COMMIT_MESSAGE="Auto-update files in target folder"
</br>#git commit -m "$COMMIT_MESSAGE" -- "$TARGET_FOLDER"
