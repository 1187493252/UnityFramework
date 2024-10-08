# UnityFramework

#源项目增加Hook文件pre-commit

#!/bin/sh

# 指定源文件夹和目标文件夹
SOURCE_FOLDER="Samples"
TARGET_FOLDER="Samples~"

# 确保目标文件夹存在
mkdir -p "$TARGET_FOLDER"

# 清空目标文件夹中的所有文件和子文件夹
rm -rf "$TARGET_FOLDER"/*

# 复制源文件夹及其内容到目标文件夹
cp -r "$SOURCE_FOLDER"/. "$TARGET_FOLDER"/

# 添加目标文件夹中的所有文件和子文件夹到暂存区
git add "$TARGET_FOLDER"

# 提交信息（可选）
#COMMIT_MESSAGE="Auto-update files in target folder"
#git commit -m "$COMMIT_MESSAGE" -- "$TARGET_FOLDER"