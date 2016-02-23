# Gu.Wpf.ScrollExtensions

```
<ListBox x:Name="IntListBox"
         Grid.Row="1"
         ItemsSource="{Binding Ints}"
         scroll:ListBoxItem.ScrolledIntoViewChanged="ListBoxItem_OnScrolledIntoViewChanged">
    <ListBox.ItemContainerStyle>
        <Style TargetType="{x:Type ListBoxItem}">
            <Setter Property="BorderBrush" Value="{x:Null}" />
            <Setter Property="BorderThickness" Value="3" />
            <Setter Property="Margin" Value="5" />
            <EventSetter Event="scroll:ListBoxItem.FirstAppearance"
                         Handler="OnFirstAppearance" />
            <Style.Triggers>
                <DataTrigger Binding="{Binding Path=(scroll:ListBoxItem.IsScrolledIntoView),
                                               RelativeSource={RelativeSource Self}}"
                             Value="{x:Static scroll:ScrolledIntoView.Fully}">
                    <Setter Property="BorderBrush" Value="HotPink" />
                </DataTrigger>

                <DataTrigger Binding="{Binding Path=(scroll:ListBoxItem.IsScrolledIntoView),
                                                RelativeSource={RelativeSource Self}}"
                             Value="{x:Static scroll:ScrolledIntoView.Partly}">
                    <Setter Property="BorderBrush" Value="Gray" />
                </DataTrigger>
            </Style.Triggers>
        </Style>
    </ListBox.ItemContainerStyle>
</ListBox>
```
